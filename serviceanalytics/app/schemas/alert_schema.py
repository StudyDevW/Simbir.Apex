from datetime import datetime
from typing import Optional
from enum import Enum

from pydantic import BaseModel

from serviceanalytics.app.schemas.event_schema import EventSchema


class AlertStatus(str, Enum):
    new = "new"
    false_positive = "false_positive"
    confirmed_preset = "confirmed_preset"
    escalated = "escalated"
    resolved = "resolved"


class AlertSchema(BaseModel):
    id: int
    ruleId: int
    assignedTo: Optional[int] = None
    hostname: str
    title: str
    description: str
    status: AlertStatus
    severity: str
    createdAt: datetime
    closedAt: Optional[datetime] = None
    resolutionNotes: Optional[str] = None
    rawData: list[EventSchema]
