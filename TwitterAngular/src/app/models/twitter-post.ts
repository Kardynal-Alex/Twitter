import { user } from "../models/user";
import { comment } from "./comment";
import { images } from "./images";

export class twitterPost{
    id:string;
    postText:string;
    dateCreation:Date;
    like:number;
    userId:string;
    user:user;
    images:images;
    comments:comment[];
}