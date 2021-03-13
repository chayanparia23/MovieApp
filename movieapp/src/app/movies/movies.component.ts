import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { MovieInfo } from '../_models/movie.model';
import { Pagination } from '../_models/pagination.model';
import { UserParams } from '../_models/userParams.model';
import { MovieService } from '../_services/movie.service';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit {

  pagination: Pagination;
  userParams: UserParams;
  paginationMaxSize = 20;

  baseUrl: string = environment.apiUrl; 
  movies: MovieInfo[] = [];
  @ViewChild('searchForm', {static: false}) searchForm: NgForm;

  constructor(private movieSrv: MovieService, private router: Router) { 
    this.userParams = this.movieSrv.getUserParams();
  }

  ngOnInit(): void {
    this.movieSrv.setUserParams(this.userParams);
    this.getMovies();
  }

  getMovies(){
    return this.movieSrv.getMovies(this.userParams).subscribe((response) =>{
      this.movies = response.result;
      this.pagination = response.pagination;
    });
  }

  pageChanged(event: any){
    this.userParams.pageNumber = event.page;
    this.movieSrv.setUserParams(this.userParams);
    this.getMovies();
  }

  search() {
     this.movieSrv.setUserParams(this.userParams);
     this.getMovies();
  }

  reset(){
    this.userParams.searchText = "";
    this.userParams.searchTextType = "";
    this.movieSrv.setUserParams(this.userParams);
    this.getMovies();
  }

  showDetails(id: number){
    this.router.navigateByUrl("/movies/"+ id);
  }

}
