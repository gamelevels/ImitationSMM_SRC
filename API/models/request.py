from dataclasses import dataclass
import datetime

@dataclass
class Request:
    Handle: str
    UsedBy: str
    StartDate: datetime
    EndDate: datetime
    EndpointType: str


