import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserSessionService } from '../../../_services/user-session.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.css'
})
export class FileUploadComponent {

  userSession = inject(UserSessionService);
  toaster = inject(ToastrService);
  file: File | null = null;

  onChange(event: any){
    const file: File = event.target.files[0];

    if(file){
      this.file = file;
    }
  }

  upload(){
    if(!this.file){
      this.toaster.error("Please select a file");
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file, this.file.name);

    this.userSession.postWatchHistory(formData)
    
  }
}
