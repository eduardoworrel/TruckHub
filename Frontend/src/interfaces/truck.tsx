export interface DashboardInfoResponse {
  total: number;
  plantCounts: PlantCount[];
  hourCounts: HourCount[];
  detailedHourCounts: DetailedHourCount[];
}

export interface DetailedHourCount {
  time: string;
  modelName: string;
  count: number;
}

export interface HourCount {
  time: string;
  count: number;
}

export interface PlantCount {
  country: string;
  count: number;
}

export type TrucksResponse = {
  id: string;
  model: string;
  manufacturingYear: number;
  chassisCode: string;
  color: string;
  plantName: string;
  createdAt: string;
};

export type TruckFormState = {
  id: string;
  model: number;
  manufacturingYear: number;
  chassisCode: string;
  color: string;
  plantName: number;
};

export type TruckDefinitions = {
  truckModels: { value: number; name: string; description: string }[];
  plantLocations: { value: number; name: string; description: string }[];
};
