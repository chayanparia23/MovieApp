import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MovieInfo } from 'src/app/_models/movie.model';
import { MovieService } from 'src/app/_services/movie.service';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css']
})
export class DetailComponent implements OnInit, OnDestroy {

  selectedMovieSubscription: Subscription;
  selectedMovieId: number;
  selectedMovie: MovieInfo;

  constructor(private route: ActivatedRoute, private movieSrv: MovieService,
    private router: Router) { }

  ngOnInit(): void {
    this.selectedMovieSubscription = this.route.params.subscribe((params: Params) => {
      this.selectedMovieId = +params['id'];
      this.movieSrv.getMovie(this.selectedMovieId).subscribe((response: MovieInfo) => {
        this.selectedMovie = response;
      });
    });
  }

  ngOnDestroy(){
    this.selectedMovieSubscription.unsubscribe();
  }

  backTomovieList(){
    this.router.navigateByUrl("/movies");
  }

}
