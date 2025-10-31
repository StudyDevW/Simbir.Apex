from enum import Enum

from pydantic import BaseModel
from uuid import UUID
from datetime import datetime


class EventCategory(str, Enum):
    unknown = "unknown"
    auth = "auth"
    file_system = "file_system"
    process = "process"
    hardware = "hardware"
    network = "network"


class EventSeverity(str, Enum):
    low = "low"
    medium = "medium"
    high = "high"


class EventSchema(BaseModel):
    id: UUID
    category: EventCategory
    ip: str
    isLan: bool
    device: str
    eventId: str
    severity: EventSeverity
    timestamp: datetime
