import { ChargingStationBackendClient } from "./generated/client.g";

const customFetch = {
    fetch: async (url: RequestInfo, init?: RequestInit) => {
        const response = await fetch(url, init);
        return response;
    }
}

// bei neuem Controller muss hier ein neuer Client erstellt werden
export const ChargerClient = new  ChargingStationBackendClient.ChargerClient("http://localhost:7067", customFetch)