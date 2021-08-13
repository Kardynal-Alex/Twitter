import { Guid } from "guid-typescript";

export interface comment{
    id:string;
    author:string;
    text:string;
    dateCreation:Date;
    twitterPostId:Guid;
    userId:string;
}