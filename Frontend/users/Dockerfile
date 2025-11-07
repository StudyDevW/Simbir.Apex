FROM node:20-alpine AS builder

WORKDIR /frontend/users

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build


FROM node:20-alpine

WORKDIR /frontend/users

COPY --from=builder /frontend/users/package*.json ./
COPY --from=builder /frontend/users/dist ./dist
COPY --from=builder /frontend/users/node_modules ./node_modules

EXPOSE 5002

CMD ["npm", "run", "start"]