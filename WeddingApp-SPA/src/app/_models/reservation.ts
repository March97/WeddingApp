export interface Reservation {

    id: number;
    placeId: number;
    userId: number;
    amountOfGuests: number;
    date: Date;
    comments: string;
}