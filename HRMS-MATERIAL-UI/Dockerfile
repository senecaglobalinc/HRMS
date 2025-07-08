FROM node:16-alpine3.16 AS builder

WORKDIR /app

COPY package*.json ./

RUN npm ci --legacy-peer-deps

COPY . .

RUN npm run build

FROM nginx:latest

COPY --from=builder /app/dist/hrms /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
