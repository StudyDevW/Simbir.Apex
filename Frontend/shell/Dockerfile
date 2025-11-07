FROM node:20-alpine AS builder

WORKDIR /frontend/shell

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build


FROM node:20-alpine

WORKDIR /frontend/shell

COPY --from=builder /frontend/shell/package*.json ./
COPY --from=builder /frontend/shell/dist ./dist
COPY --from=builder /frontend/shell/node_modules ./node_modules

EXPOSE 5000

CMD ["npm", "run", "start"]