from datetime import datetime
from enum import Enum

from pydantic import BaseModel

class ResponseSeverity(str, Enum):
    low = "low"
    medium = "medium"
    high = "high"
    critical = "critical"

class ResponseSchema(BaseModel):
    probability: float
    predicted_for: datetime
    severity: ResponseSeverity
    threat_type: str
    guidelines: list[str]