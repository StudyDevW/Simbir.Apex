FROM node:20-alpine AS builder

WORKDIR /frontend/analytics

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build


FROM node:20-alpine

WORKDIR /frontend/analytics

COPY --from=builder /frontend/analytics/package*.json ./
COPY --from=builder /frontend/analytics/dist ./dist
COPY --from=builder /frontend/analytics/node_modules ./node_modules

EXPOSE 5005

CMD ["npm", "run", "start"]