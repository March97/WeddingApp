export interface Reservation {

    id: number;
    placeId: number;
    userId: number;
    amountOfGuests: number;
    cost: number;
    date: Date;
    comments: string;
    placeName?: string;
    userName?: string;
}