import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Job } from './job';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { User } from '../models/user';


@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrls: ['./jobs.component.css']
})

export class JobsComponent {
  public displayedColumns: string[] = ['id','name', 'description', 'momentTime', 'periodicity', 'delete'];
  public jobs: Job[];
  job: Job;
  email: Text;
  id?: number;


  constructor(
    private http: HttpClient,
    private router: Router,
    private userService: UserService,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.http.get<Job[]>(this.baseUrl + 'api/jobs')
      .subscribe(result => {
        this.jobs = result;
      }, error => console.error(error));

    this.userService.getEmail().subscribe(
      (result: Text) => {
        this.email = result
      }
    );
  
  }
 
  delete(j: Job) {
    var url = this.baseUrl + "api/jobs/" + j.id;
    this.http.delete<Job>(url).subscribe(result => {
      console.log("Job " + j.name + " has been deleted.");

      window.location.reload();
    }, error => console.error(error));
  }
  
}
