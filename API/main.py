# Built to simulate a real third-party API
# However, for project purposes this is built to return dummy server-sided data
# If this was a served API there would be API keys

import json
from fastapi import FastAPI
from fastapi import Request as Req
from random import randint
from models.request import Request
from models.thunder import Thunder
from datetime import datetime, timedelta

app = FastAPI()
api = Thunder()

adminKey = ""
sanitize = ["%", "<", ">", ".css", ".js", ";", "*", "(", ")", "|", "-"]
services = {
    "followers": 817,
    "likes": 2159,
    #"likes": 2214
    #"comments": 1658
}

history: list[Request] = []

def response(message: str, result: bool):
    return {"result": result, "message": message}

# Simulate multiple endpoints
@app.get("/smm/v1/")
@app.get("/smm/v2/")
async def smm(handle: str, fillType: str, requests: int, request: Req):
    if any(x == "" for x in [handle, fillType, requests]):
        return response("Invalid parameters", False)

    if any([(x, y) for x in sanitize for y in [handle.lower(), fillType.lower()] if x in y]):  
        return response("u dumb? lmao", False)

    if requests > 25000:
        return response("Request count too high", False)
    
    if fillType not in ["followers", "likes", "comments", "subscribers", "repost", "custom", "shares"]:
        return response("Invalid fill type", False)
    
    if not any(x in handle for x in ["https", "www"]):
        return response("Invalid handle", False)
    
    endpoint = request.url.path
    check = [x for x in history if x.EndDate > datetime.now() and x.EndpointType == endpoint]
    if len(check) != 0:
        return response("Previous request to this handle has not finished, please wait", False)

    try:
        if "instagram" in handle:
            serviceID = services.get(fillType)
            if serviceID is not None:
                #Only hard coding username because this wasn't originally built
                #For a third party API, I added last minute and don't want to rebuild logic
                d = {
                    'service': serviceID,
                    'link': handle,
                    'quantity': requests,
                    "username": "imitationsmm" 
                }
                if fillType == "likes":
                    d['media'] = handle

                req = await api.order(d)
                print(f"API Response: {req}\nHandle: {handle}\nService: {serviceID}\nQuantity: {requests}")
                
        history.append(Request(handle, requests, datetime.now(), datetime.now() + timedelta(seconds=60), endpoint))
        return response(f"Request sent, {handle} with {fillType} for {requests} requests", True)
    except:
        return response(f"Request failed to send, API offline", False)

@app.get("/cancel/")
async def smm(handle: str, endpoint: str):
    if handle == "":
        return response("Invalid parameters", False)
    
    try:
        check = [x for x in history if x.EndDate > datetime.now() and endpoint in x.EndpointType]
        if len(check) == 0:
            return response(f"No running concurrent on {handle}", False)

        else:
            history.remove(check[0])
            return response(f"Request to {handle} canceled", True)
    except:
        return response("An unexpected error has occured", False)

    
@app.get("/stats/v1/")
async def stats():
    return {"accounts": randint(250000,2000000)}
    

@app.get("/stats/balance")
async def balance(key: str):
    if key == None or key != adminKey:
        return response("Invalid request", False)
    
    bal = await api.balance()
    return {"balance": bal['balance']}


@app.get("/")
async def index():
    return {"message": "invalid endpoint"}