import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';
import { User } from '../models/user';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css']
})
export class AdminHomeComponent {
  public displayedColumns: string[] = ['email', 'firstName'];
  public users: User[];
  adminData: string;

  constructor(private userService: UserService,
private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string  ) { }

  ngOnInit() {
    this.userService.getAdminData().subscribe(
      (result: User[]) => {
        this.users = result;
      }
    );
  }

  fetchAdminData() {
    this.userService.getAdminData().subscribe(
      (result: User[]) => {
        this.users = result;
      }
    );
  }


}
