# Not handling fillIDs, so won't implement cancel functionality

import httpx

class Thunder():
    def __init__(self) -> None:
        self.api = 'https://thundersmmpanel.com/api/v2'
        self.key = ''
        self.client = httpx.AsyncClient()
        self.client.headers = {
            'content-type': 'application/x-www-form-urlencoded',
            'user-agent': 'Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)'
        }

    async def POST(self, action, data):
        data = {
            'key': self.key,
            'action': action,
            **data
        }
        resp = await self.client.post(self.api, data=data)
        if resp.status_code == 200:
            return resp.json()
        else:
            return {"error": f"Failed to send API request, status code: {resp.status_code}"}

    async def order(self, info):
        return await self.POST('add', info)

    async def balance(self):
        return await self.POST('balance', {})
