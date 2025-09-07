# Stage 1: Build Angular app
FROM node:20 AS build
WORKDIR /app

# Copy package files and install dependencies with retries
COPY package*.json ./
RUN npm ci --retry=5 --fetch-retries=5 --fetch-retry-factor=2 \
    --fetch-retry-mintimeout=20000 --fetch-retry-maxtimeout=120000

# Copy source and build Angular
COPY . .
RUN npm run build -- angular-app-5 --configuration production

# Stage 2: Serve with nginx
FROM nginx:alpine
COPY --from=build /app/dist/angular-app-5/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
