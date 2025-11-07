FROM node:20-alpine AS builder

WORKDIR /frontend/events

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build


FROM node:20-alpine

WORKDIR /frontend/events

COPY --from=builder /frontend/events/package*.json ./
COPY --from=builder /frontend/events/dist ./dist
COPY --from=builder /frontend/events/node_modules ./node_modules

EXPOSE 5001

CMD ["npm", "run", "start"]