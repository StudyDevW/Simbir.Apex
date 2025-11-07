FROM node:20-alpine AS builder

WORKDIR /frontend/rules

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build


FROM node:20-alpine

WORKDIR /frontend/rules

COPY --from=builder /frontend/rules/package*.json ./
COPY --from=builder /frontend/rules/dist ./dist
COPY --from=builder /frontend/rules/node_modules ./node_modules

EXPOSE 5003

CMD ["npm", "run", "start"]