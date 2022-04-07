import { Component, Input, ViewChild, ViewContainerRef, OnInit, Inject } from '@angular/core';

import { ImageService } from './image.service';
import { AppComponent } from './app.component';

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

@Component({
    selector: 'upload',
    templateUrl: './upload.component.html',
    styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {

    selectedFile: File
    resultImage: Image;
    errorVisibility: string;
    successVisibility: string;
    infoVisibility: string;
    info2Visibility: string;
    fileName: string;
    uploadEnabled: boolean;
    labelText: string;
    elapsedTime: Number;

    constructor(@Inject(AppComponent) private parent: AppComponent, private imageService: ImageService) { }

    ngOnInit() {
        this.errorVisibility = "none";
        this.successVisibility = "none";
        this.infoVisibility = "none";
        this.info2Visibility = "none";
        this.uploadEnabled = false;
        this.labelText = "Choose a file.";
    }

    ngAfterContentInit() {
    }


    onFileChanged(event) {
        this.selectedFile = event.target.files[0];

        if (this.selectedFile != null) {
            this.info2Visibility = "block";
            this.fileName = this.selectedFile.name;
            this.labelText = this.selectedFile.name;
        } else {
            this.info2Visibility = "none";
            this.labelText = "Choose a file.";
        }
    }

    onUpload() {
        var fileReader = new FileReader();
        fileReader.onload = (event) => {
            //let arrayBuffer = (event.target).result;
            let arrayBuffer = fileReader.result;

            var image: Image = new Image;
            image.name = this.selectedFile.name;
            image.type = this.selectedFile.type;
            image.imageData = new Uint8Array(arrayBuffer as ArrayBuffer);
            var t0 = performance.now();

            
            this.info2Visibility = "none";
            this.infoVisibility = "block";

            var observableImage = this.imageService.analyze(image);
            observableImage.subscribe((image) => {
                var t1 = performance.now();
                this.elapsedTime = (t1 - t0) / 1000;    

                this.resultImage = image;
                this.successVisibility = "block";
                this.infoVisibility = "none";
                this.parent.fetchImages();
            
            })
        };




        fileReader.readAsArrayBuffer(this.selectedFile);


    }

}