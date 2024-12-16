import { Component, OnInit } from '@angular/core';
import { GoogleMapsModule } from '@angular/google-maps';
import { MapService } from '../_services/map.service';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-maps',
  imports: [GoogleMapsModule, NgFor],
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent implements OnInit {
  center: google.maps.LatLngLiteral = { lat: -25, lng: -49 };
  zoom = 6;

  markers: google.maps.LatLngLiteral[] = [];

  constructor(private mapService: MapService) { }

  ngOnInit(): void {
    this.mapService.mapState$.subscribe((state) => {
      this.center = { lat: state.lat, lng: state.lng };
      this.zoom = state.zoom;

      this.markers = [{lat: state.lat, lng: state.lng}]
    });
  }

  moveMap(event: google.maps.MapMouseEvent) {
    if (event.latLng) {
      const clickedLatLng = { lat: event.latLng.lat(), lng: event.latLng.lng() };
      this.markers = [clickedLatLng];
    }
  }

  getMarkerTitle(marker: google.maps.LatLngLiteral): string {
    return `Lat: ${marker.lat}, Lng: ${marker.lng}`;
  }
}
