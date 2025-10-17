from pydantic import BaseModel
from uuid import UUID
from datetime import datetime


class EventSchema(BaseModel):
    id: UUID
    category: str
    ip: str
    isLan: bool
    device: str
    eventId: str
    severity: str
    timestamp: datetime
