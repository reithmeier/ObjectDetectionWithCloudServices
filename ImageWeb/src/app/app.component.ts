import { Component, OnInit } from '@angular/core';
import { ImageService } from './image.service';
import { element } from 'protractor';

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
}

class ImageWithData {
  image: Image;
  data: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'ImageWeb';
  images: Image[] = [];
  requestTime: number;
  newVoteTopic: string = '';
  loading = true;

  constructor(private imageService: ImageService) { }

  ngOnInit(): void {
    this.loading = false;
    this.fetchImages();
  }

  fetchImages(): void {
    this.loading = true;
    this.imageService.getImages().subscribe(imgs => {
      this.images = imgs;
      this.loading = false;
    });
  }


  

  /*
  fetchImages(): void {
    this.imageService.getImages().subscribe(imgs => {
      for(let img of imgs){
        var d: ImageWithData = new ImageWithData;
        d.image = img;
        const reader = new FileReader();
        reader.onload = (e) => d.data = e.target.result;
        reader.readAsDataURL(new Blob([img.imageData]));
    
        this.images.push(d)
      }
    });
  }
  renderImage(imageData: Uint8Array): any {
    result:Object;
    const reader = new FileReader();
    reader.onload = (e) => result = e.target.result;
    reader.readAsDataURL(new Blob([imageData]));
    return result
  }
  */
}
