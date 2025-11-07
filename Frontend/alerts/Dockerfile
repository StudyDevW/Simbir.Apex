FROM node:20-alpine AS builder

WORKDIR /frontend/alerts

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build


FROM node:20-alpine

WORKDIR /frontend/alerts

COPY --from=builder /frontend/alerts/package*.json ./
COPY --from=builder /frontend/alerts/dist ./dist
COPY --from=builder /frontend/alerts/node_modules ./node_modules

EXPOSE 5004

CMD ["npm", "run", "start"]