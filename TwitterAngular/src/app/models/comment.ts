import { Guid } from "guid-typescript";

export class comment{
    id:string;
    author:string;
    text:string;
    dateCreation:Date;
    twitterPostId:Guid;
    userId:string;
    profileImagePath:string;
}