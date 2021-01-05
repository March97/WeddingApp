import { Component, Input, OnInit } from '@angular/core';
import { Place } from 'src/app/_models/place';

@Component({
  selector: 'app-place-list-card',
  templateUrl: './place-list-card.component.html',
  styleUrls: ['./place-list-card.component.css']
})
export class PlaceListCardComponent implements OnInit {
  @Input() place: Place;

  constructor() { }

  ngOnInit() {
  }

}
