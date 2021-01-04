import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/_models/photo';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-place-photo-editor',
  templateUrl: './place-photo-editor.component.html',
  styleUrls: ['./place-photo-editor.component.css']
})
export class PlacePhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Input() placeId: number;
  @Output() getMemberPhotoChange = new EventEmitter();
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
      this.uploader = new FileUploader({
        url: this.baseUrl += 'users/' + this.authService.decodedToken.nameid + '/places/'+ this.placeId + '/photos',
        authToken: 'Bearer ' + localStorage.getItem('token'),
        isHTML5: true,
        allowedFileType: ['image'],
        removeAfterUpload: true,
        autoUpload: false,
        maxFileSize: 10 * 1024 * 1024
      });
    
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
        if (photo.isMain) {
          this.getMemberPhotoChange.emit(null);
        }
      }
    };
  }

  setMainPhoto(photo: Photo) {
    this.userService.setMainPhotoForPlace(this.authService.decodedToken.nameid, this.placeId, photo.id).subscribe(() => {
      this.currentMain = this.photos.filter( p => p.isMain === true)[0];
      this.currentMain.isMain = false;
      photo.isMain = true;
      this.getMemberPhotoChange.emit(null);
    }, error => {
      this.alertify.error(error);
    });
  }

  deletePhoto(id: number) {
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.userService.deletePhotoForPlace(this.authService.decodedToken.nameid, this.placeId, id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        this.alertify.success('Photo has been deleted');
      }, error => {
        this.alertify.error('Failed to delete the photo');
      });
    });
  }

}
