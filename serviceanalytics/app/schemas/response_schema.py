from pydantic import BaseModel


class ResponseSchema(BaseModel):
    probability: float
    prediction: int
