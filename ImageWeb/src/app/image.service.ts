import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

class DetectedObject {
  x: number;
  y: number;
  w: number;
  h: number;
  objectProperty: string;
  confidence: number;
}

class Image {
  id: string;
  name: string;
  imageData: Uint8Array;
  analysisResults: DetectedObject[];
  type: string;
}


@Injectable({
  providedIn: 'root'
})
export class ImageService {

  imageServiceUrl : string="https://clc3functions.azurewebsites.net/api/images";//"http://localhost:7071/api/images";//

  constructor(private httpClient: HttpClient) { }

  getImages(): Observable<Image[]> {
    return this.httpClient.get<Image[]>(this.imageServiceUrl);
  }

  addVoteItem(item: Image): Observable<Image> {
    return this.httpClient.post<Image>(this.imageServiceUrl, item);
  }

  analyze(image: Image): Observable<Image> {
    return this.httpClient.post<Image>(`${this.imageServiceUrl}/analyze`, image);
  }


  getHello(): Observable<string> {
    return this.httpClient.get<string>(`${this.imageServiceUrl}/hello`);
  }

}
