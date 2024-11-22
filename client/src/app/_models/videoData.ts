import { CategoryData } from "./categoryData";
import { ChannelData } from "./channelData";

export interface VideoData{
    apI_Id: string;
    title: string;
    channel: ChannelData;
    published: string | null;
    category: CategoryData | null;
    topics: string[] | null;
    thumbnail: string | null;
    duration: string | null;
    views: string | null;
    dataFetched: string | null;
    retrieved: boolean;

}