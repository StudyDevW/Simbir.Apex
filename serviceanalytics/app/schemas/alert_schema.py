from datetime import datetime
from typing import Optional

from pydantic import BaseModel

from serviceanalytics.app.schemas.event_schema import EventSchema


class AlertSchema(BaseModel):
    id: int
    ruleId: int
    assignedTo: Optional[int] = None
    hostname: str
    title: str
    description: str
    status: str
    severity: str
    createdAt: datetime
    closedAt: Optional[datetime] = None
    resolutionNotes: Optional[str] = None
    rawData: list[EventSchema]