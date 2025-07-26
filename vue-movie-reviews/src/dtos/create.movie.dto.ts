export interface CreateMovieDto {
  title: string;
  description: string | null;
  directorFirstName: string;
  directorLastName: string;
  dateOfPremiere: string | null;
}
