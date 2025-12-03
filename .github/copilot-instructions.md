# AI Coding Agent Instructions for app-5

## Architecture Overview

This is a full-stack microservices application deployed on Kubernetes with GitOps:

- **Frontend**: Angular 17 standalone app (`angular-app-5/`) served via nginx
- **Backend**: .NET 9.0 Web API (`dotnet-app-5/`) with Entity Framework Core
- **Deployment**: Kubernetes manifests in `k8s/` managed by ArgoCD
- **Container Registry**: Docker Hub (`dokkaushik/angular-app-5`, `dokkaushik/dotnet-app-5`)

## Service Communication

- **Development**: Angular calls `http://localhost:5000/` (docker-compose setup)
- **Production**: Nginx ingress routes `/api/*` to backend, `/` to frontend at `karanam-k2.duckdns.org`
- **CORS Configuration**: Backend explicitly allows `http://angular5.138.2.131.203.nip.io:31787` and `:80` variant
- **API Pattern**: Frontend calls `/api/My/add-numbers` endpoint using `environment.apiUrl + 'My/add-numbers'`

## Project-Specific Conventions

### Backend (.NET)
- **Namespace Mix**: Uses both `DotnetApp5` (models/context) and `GherkinHome.Services` (services) - maintain this pattern
- **Service Layer**: All business logic goes through `IMyService` interface injected via DI
- **Database**: SQL Server configured via `DefaultConnection` connection string (currently unused but wired up)
- **Port**: Always runs on port 5000, set via `ASPNETCORE_URLS=http://+:5000`
- **Entry Point**: Static `CreateHost()` method in `Program.cs` for testability

### Frontend (Angular)
- **Standalone Components**: Uses Angular 17 standalone pattern (`standalone: true`, imports in component)
- **Environment Files**: Toggle between `environment.ts` (dev) and `environment.prod.ts` (prod)
- **SSR Support**: Includes server-side rendering setup (`server.ts`, `serve:ssr` script) but uses nginx in containers
- **HTTP Module**: Import `HttpClientModule` directly in standalone components

## Critical Workflows

### Local Development
```powershell
# Start both services
docker-compose up --build

# Access: Frontend at :4200, Backend at :5000
```

### Build & Deploy
```powershell
# Build frontend (multi-stage with npm cache)
cd angular-app-5; docker build -t dokkaushik/angular-app-5:TAG .

# Build backend (multi-stage .NET SDK → runtime)
cd dotnet-app-5; docker build -t dokkaushik/dotnet-app-5:TAG .

# Update k8s/kustomization.yaml newTag with git commit SHA
# ArgoCD auto-syncs from main branch
```

### Testing
- **Frontend**: `npm test` (Karma/Jasmine)
- **Backend**: No test project present (add under `dotnet-app-5/` if needed)

## Key Integration Points

- **Kustomize**: `k8s/kustomization.yaml` controls image tags (currently `46a4b1c736d885c79d0712481e62465889a6215b`)
- **TLS**: Uses cert-manager with letsencrypt cluster issuer
- **Namespace**: All resources deploy to `app-5-dev`
- **Health Checks**: Commented out in deployments but paths are `/ (frontend)` and `/health (backend)`

## When Modifying

- **API Endpoints**: Update both `MyController.cs` routes AND Angular service calls
- **CORS Origins**: Sync `Program.cs` CORS policy with actual ingress hostnames
- **Image Tags**: After Docker push, update `kustomization.yaml` newTag to trigger ArgoCD sync
- **Environment URLs**: Production uses relative `/api/` path; dev uses absolute `http://localhost:5000/`
