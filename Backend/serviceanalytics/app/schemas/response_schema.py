from pydantic import BaseModel


class ResponseSchema(BaseModel):
    probability: float
    guidelines: list[str] # TODO: определить какие будут рекомендации от ЭС
