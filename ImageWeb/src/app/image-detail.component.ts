import { Component, OnInit, Input, ViewChild, ViewContainerRef, ElementRef, AfterViewInit, AfterContentInit, AfterViewChecked, Renderer2 } from '@angular/core';

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
    selector: 'image-detail',
    templateUrl: './image-detail.component.html',
    styleUrls: ['./image-detail.component.css']
})
export class ImageDetailComponent implements OnInit {
    @Input() image: Image;

    constructor() {
    }

    ngOnInit() {
    }

}