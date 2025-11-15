# Stage 1: Build Angular app with cache
FROM node:20 AS build
WORKDIR /app

# Leverage Docker cache for deps
COPY package*.json ./
# Use an npm cache mount to speed up installs in CI
RUN --mount=type=cache,target=/root/.npm npm ci

# Copy source and build Angular
COPY . .
# Build the default project in production
RUN npm run build -- --configuration production

# Stage 2: Serve with nginx
FROM nginx:1.27-alpine
# Basic gzip + SPA config lives here
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/angular-app-5 /usr/share/nginx/html
EXPOSE 80
