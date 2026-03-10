export type GoalType =
  | 'LeadGeneration'
  | 'Authority'
  | 'Storytelling'
  | 'Engagement';

export interface GenerationListItem {
  id: string;
  goalType: GoalType;
  inputText: string;
  outputText: string;
  createdAt: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface DashboardResponse {
  credits: number;
  generationsCount: number;
  recentGenerations: GenerationListItem[];
}

export interface GeneratePostRequest {
  inputText: string;
  goalType: GoalType;
}

export interface GeneratePostResponse {
  outputText: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
}

export interface MeResponse {
  email: string;
  credits: number;
}
