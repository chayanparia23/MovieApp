import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from '../_helpers/pagination';
import { MovieInfo } from '../_models/movie.model';
import { UserParams } from '../_models/userParams.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  baseUrl: string = environment.apiUrl;
  private userParams: UserParams;
  movies: MovieInfo[] = []; 
  moviesCache = new Map();
  
  constructor(private http: HttpClient) { 
    this.userParams = new UserParams();
  }

  getUserParams(){
    return this.userParams;
  }

  setUserParams(userParams: UserParams){
    this.userParams = userParams;
  }

  getMovies(userParams: UserParams){
    //From Caching
    var response = this.moviesCache.get(Object.values(userParams).join('-'));
    if(response){
      return of(response);
    }

    //Else get from server
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append('searchText', userParams.searchText);
    params = params.append('searchTextType', userParams.searchTextType);
    return getPaginatedResult<MovieInfo[]>(this.baseUrl+"movies", params, this.http).pipe(map(response => {
      this.moviesCache.set(Object.values(userParams).join('-'), response);
      this.movies = response.result;
      return response;
    }));
  }

  getMovie(movieId: number){
    //From caching
    const movie = [...this.moviesCache.values()]
            .reduce((arr, elem) => arr.concat(elem.result), [])
            .find((movie: MovieInfo) => movie.id === movieId);
    if(movie){
      return of(movie);
    }

    //Else from server
    return this.http.get<MovieInfo>(this.baseUrl+'movies/'+ movieId);
  }
}
