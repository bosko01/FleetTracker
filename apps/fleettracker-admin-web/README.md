# FleetTracker Admin Web (React + Vite + TypeScript)

## Setup
1. Copy `.env.example` to `.env`.
2. Set `VITE_API_BASE_URL` (localhost or ngrok URL).
3. Install and run:
   - `npm install`
   - `npm run dev`

## API base URL configuration
- The frontend reads the API base URL from `VITE_API_BASE_URL`.
- Default local backend value:
  - `VITE_API_BASE_URL=https://localhost:57337`
- To use a tunnel (for example ngrok), set:
  - `VITE_API_BASE_URL=https://abc123.ngrok-free.app`
- If local HTTPS API calls fail, open `https://localhost:57337/swagger` in your browser and accept the development certificate warning.

## Build
- `npm run build`
- `npm run preview`
