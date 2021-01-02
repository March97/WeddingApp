/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PlacesCreateComponent } from './place-create.component';

describe('PlacesCreateComponent', () => {
  let component: PlacesCreateComponent;
  let fixture: ComponentFixture<PlacesCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlacesCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlacesCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
