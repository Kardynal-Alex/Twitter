import { Guid } from "guid-typescript";

export interface comment{
    id:Guid;
    author:string;
    text:string;
    dateCreation:Date;
    twitterPostId:Guid;
    userId:string;
}