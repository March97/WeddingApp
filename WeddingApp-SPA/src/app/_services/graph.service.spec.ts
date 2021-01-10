/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { GraphService } from './graph.service';

describe('Service: Graph', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GraphService]
    });
  });

  it('should ...', inject([GraphService], (service: GraphService) => {
    expect(service).toBeTruthy();
  }));
});
