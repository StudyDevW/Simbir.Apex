FROM node:20-alpine AS builder

WORKDIR /frontend/reports

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build


FROM node:20-alpine

WORKDIR /frontend/reports

COPY --from=builder /frontend/reports/package*.json ./
COPY --from=builder /frontend/reports/dist ./dist
COPY --from=builder /frontend/reports/node_modules ./node_modules

EXPOSE 5006

CMD ["npm", "run", "start"]