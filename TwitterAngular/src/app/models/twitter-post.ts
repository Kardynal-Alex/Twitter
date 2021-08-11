import { Guid } from "guid-typescript";
import { user } from "../models/user";
import { images } from "./images";

export interface twitterPost{
    id:Guid;
    postText:string;
    dateCreation:Date;
    like:number;
    userId:string;
    user:user;
    images:images;
}