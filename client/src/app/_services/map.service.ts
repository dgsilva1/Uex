import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MapService {
  private mapStateSource = new BehaviorSubject<{ lat: number; lng: number; zoom: number }>({
    lat: -25,
    lng: -49,
    zoom: 6,
  });
  mapState$ = this.mapStateSource.asObservable();

  setCenter(lat: number, lng: number, zoom: number) {
    this.mapStateSource.next({ lat, lng, zoom });
  }
}
