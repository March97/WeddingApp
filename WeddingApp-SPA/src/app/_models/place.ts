import { Photo } from "./photo";

export interface Place {

    id: number;
    name: string;
    country: string;
    city: string;
    address: string;
    facilities?: string;
    capacity: number;
    price: number;
    inPrice?: string;
    bonuses?: string;
    description?: string;
    photos?: Photo[];
}