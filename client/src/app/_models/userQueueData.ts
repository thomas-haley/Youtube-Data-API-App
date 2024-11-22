import { UserData } from "./userData";

export interface UserQueueData{
    id: number;
    user: UserData;
    taskType: string;
    videos: string[] | null;
    channels: string[] | null;
    categories: string[] | null;
    canceled: boolean;
    completed: boolean;
}