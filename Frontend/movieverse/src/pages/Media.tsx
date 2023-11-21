import React, { Suspense } from "react";
import { useParams } from "react-router-dom";
import { SyntheticEvent, useCallback, useEffect, useState } from "react";
import { NotFound } from "../components/basic/NotFound";
import { useUserMediaInfo } from "../hooks/useUserMediaInfo";
import { MediaInfoDto } from "../core/dtos/user/mediaInfoDto";
import { useMedia } from "../hooks/useMedia";
import { Loading } from "../components/basic/Loading";
import { Icon } from "../components/basic/Icon";
import { RateMediaPopup } from "../components/media/RateMediaPopup";
import { ContentViewer } from "../components/content/ContentViewer";
import { MovieDto, SeriesDto } from "../core/dtos/media/mediaDto";
import { Button } from "../components/basic/Button";
import { Section } from "../components/basic/Section";
import { Text } from "../components/basic/Text";
import { useUserRoles } from "../hooks/useUserRoles";
import { Platforms } from "../components/platform/Platforms";
import { Player } from "../common/player";
import { UserRoles } from "../UserRoles";
import { CloudStore } from "../CloudStore";
import { EditMedia } from "../components/media/EditMedia";
import { Api } from "../Api";
import "./Media.css";
import Blank from "../assets/blank.png";
import NoVideo from "../assets/no-video.svg";
import YouTube from "react-youtube";
import Star from "../assets/star.svg";
import Position from "../assets/position.svg";
import Plus from "../assets/plus.svg";
import CheckGold from "../assets/check-gold.svg";
import StarEmpty from "../assets/star-empty.svg";
import Images from "../assets/images.svg";
import Pen from "../assets/pen.svg";

// Dynamic imports
const LazyGenres = React.lazy(() => import("../components/genre/Genres"));
const LazyStaff = React.lazy(() => import("../components/staff/Staff"));
const LazyStatistics = React.lazy(() => import("../components/media/Statistics"));
const LazyReviews = React.lazy(() => import("../components/media/Reviews"));

// Media page
export const Media: React.FC = () => {
  const params = useParams();
  const [loading, setLoading] = useState<boolean>(true);
  const [media] = useMedia(params.id ?? "");
  const [userMediaInfo, setUserMediaInfo] = useUserMediaInfo(params.id ?? "");
  const [imgSrc, setImgSrc] = useState<string>(Blank);
  const [videoId, setVideoId] = useState<string | null>(null);
  const [isRatingPopupOpen, setIsRatingPopupOpen] = useState<boolean>(false);
  const [isContentPopupOpen, setIsContentPopupOpen] = useState<boolean>(false);
  const [userRoles] = useUserRoles();
  const [editMode, setEditMode] = useState<boolean>(false);
  
  // On media change
  useEffect(() => {
    if (media) {
      document.title = `${media.title} - Movieverse`;

      media.posterId ? setImgSrc(CloudStore.getImageUrl(media.posterId as string)) : setImgSrc(Blank);
      if (media.trailerId) {
        Api.getVideoPath(media.trailerId).then(url => {
          const video = url.split("v=")[1];
          setVideoId(video);
        });
      }

      setLoading(false);
    }
    else {
      if (!loading) {
        document.title = "Not found - Movieverse";
      }
    }
  }, [media]);

  // Error handling
  const onImgError = useCallback((e: SyntheticEvent<HTMLImageElement, Event>): void => {
		const img = e.target as HTMLImageElement;
		img.src = NoVideo;
	}, []);

  // Calculate popularity
  const calculatePopularity = useCallback((): string => {
    const position = media?.currentPosition ?? 0;    
    return position === 0 ? "-" : position.toString();
  }, [media?.currentPosition]);


  // Handlers
  const watchlistHandler = useCallback(() => {
    Api.updateWatchlistStatus(params.id as string)
    .then((res) => {
      if (res.ok) {
        const mediaInfo: MediaInfoDto = {
          isOnWatchlist: !userMediaInfo?.isOnWatchlist ?? true,
          rating: userMediaInfo?.rating ?? 0
        };
        setUserMediaInfo(mediaInfo);
      }				
    });
  }, [userMediaInfo]);

  const ratingPopupHandler = useCallback(() => {
    setIsRatingPopupOpen(!isRatingPopupOpen);
  }, [isRatingPopupOpen]);

  const ratingHandler = useCallback((event: SyntheticEvent<Element, Event>, value: number | null) => {
    Api.updateRating(params.id as string, value ?? 0)
      .then((res) => {
        if (res.ok) {
          const mediaInfo: MediaInfoDto = {
            isOnWatchlist: userMediaInfo?.isOnWatchlist ?? false,
            rating: value ?? 0
          };
          setUserMediaInfo(mediaInfo);
        }				
      });
  }, [userMediaInfo]);

  const contentHandler = useCallback(() => {
    setIsContentPopupOpen(!isContentPopupOpen);
  }, [isContentPopupOpen]);

  //Toggle edit mode
  const toggleEditMode = useCallback(() => {
    setEditMode(!editMode);
  }, [editMode]);

  // Render utils
  const isMovie = useCallback((media: MovieDto | SeriesDto | null): media is MovieDto => {
    return media !== null && "sequelId" in media && "prequelId" in media ;
  }, []);

  const isSeries = useCallback((media: MovieDto | SeriesDto | null): media is SeriesDto => {
    return media !== null && "seasonCount" in media && "episodeCount" in media;
  }, []);

  const renderUtils = useCallback((): JSX.Element => {
    if(isMovie(media)) {
      return (
        <>
          <div className="media-utils-stats">
            {
              media.sequelId &&
              <Button label={`Sequel > ${media.sequelTitle}`}
                      color="dark"
                      redirect={`/media/${media.sequelId}`}
              />
            }
            {            
              media.prequelId &&
              <Button label={`Prequel < ${media.prequelTitle}`}
                      color="dark"
                      redirect={`/media/${media.prequelId}`}
              />
            }            
          </div>
          <Platforms className="media-utils-platforms"
                     mediaId={params.id as string}
          />
        </>
      );
    }

    if(isSeries(media)) {
      return (
        <>
          <div className="media-utils-stats">
            <div className="media-series">
              {
                media.seriesEnded &&
                <>
                  <Text label="Series ended" 
                        text={`${media.seriesEnded.substring(0, 10)}`} 
                  />
                  <hr className="media-break" />
                </>
              }
              <div className="media-series-info">
                <Text className="media-series-data" 
                      label="Seasons" 
                      text={`${media.seasonCount}`} 
                />
                <Text className="media-series-data"
                      label="Episodes" 
                      text={`${media.episodeCount}`} 
                />
              </div>
            </div>
            <Button label="View episodes"
                    color="dark"
                    redirect={`/media/${params.id}/episodes`}
            />
          </div>
          <Platforms className="media-platforms"
                     mediaId={params.id as string}
          />
        </>
      )
    }    
    return <></>;
  }, [media]);

  // Render
  return (
    <>      
      {
        loading ? <Loading /> :
        !media ? <NotFound /> : 
        <div className="media-page">
          {
            (userRoles?.includes(UserRoles.Administrator) || userRoles?.includes(UserRoles.SystemAdministrator)) &&
            <>
              <img className="person-pen"
                  src={Pen} 
                  alt="pen"
                  onClick={toggleEditMode}
              />              
                {
                  editMode &&
                  <EditMedia mediaId={params.id as string}
                             onClose={toggleEditMode}
                  />
                }
            </>
          }
          <span className="media-title">{media.title}</span>
          <div className="media-stats">
            <Icon className="media-icon-horizontal"
                  label="RATING"
                  src={Star}
                  text={media.basicStatistics.rating.toString()}
            />
            <Icon className="media-icon-horizontal"
                  label="POPULARITY"      
                  src={Position}            
                  text={calculatePopularity()}
            />
          </div>          
          <div className="media-clickable">
            <Icon className="media-icon-vertical"
                  label="WATCHLIST"      
                  src={userMediaInfo?.isOnWatchlist ? CheckGold : Plus}  
                  onClick={watchlistHandler}          
            />
            <Icon className="media-icon-vertical"
                  label="YOUR RATING"      
                  src={userMediaInfo?.rating ? Star : StarEmpty}            
                  text={userMediaInfo?.rating ? userMediaInfo.rating.toString() : ""}
                  onClick={ratingPopupHandler}
            />
            <Icon className="media-icon-vertical"
                  label="MORE CONTENT"      
                  src={Images}
                  onClick={contentHandler}           
            />
          </div>
          <img className="media-poster"
               src={imgSrc}
               onError={onImgError}
               alt={media.title}
          />
          <div className="media-trailer">
            {
              videoId ? 
              <YouTube videoId={videoId}
                       iframeClassName="media-page-video"
                       opts={Player.opts}
              /> :
              <img className="media-no-trailer"
                   src={NoVideo}
                   onError={onImgError}
                   alt="No trailer available" 
              />
            }
          </div>
          <div className="media-description">            
            <Section title="Storyline">
              <span>{media.details.storyline ?? "No data."}</span>
            </Section>
          </div>
          <div className="media-utils">
            {renderUtils()}
          </div>
          <div className="media-details">
            <Section title="Details">
              <Text label={`Runtime`} text={media.details.runtime ? `${media.details.runtime} min` : "unknown"} />
              <hr className="media-break" />
              <Text label={`Certificate`} text={`${media.details.certificate ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Release date`} text={`${media.details.releaseDate?.substring(0, 10) ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Tagline`} text={`${media.details.tagline ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Country of origin`} text={`${media.details.countryOfOrigin ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Language`} text={`${media.details.language ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Filming locations`} text={`${media.details.filmingLocations ?? "unknown"}`} />        
            </Section>
          </div>
          <div className="media-specs">
            <Section title="Technical specs">
              <Text label={`Aspect ratio`} text={`${media.technicalSpecs.aspectRatio ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Camera`} text={`${media.technicalSpecs.camera ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Color`} text={`${media.technicalSpecs.color ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Sound mix`} text={`${media.technicalSpecs.soundMix ?? "unknown"}`} />
              <hr className="media-break" />
              <Text label={`Negative format`} text={`${media.technicalSpecs.negativeFormat ?? "unknown"}`} />
              <hr className="media-break" />
            </Section>
          </div>
          <div className="media-genre">
            <Section title="Genres">
              <Suspense fallback={<Loading />}>
                <LazyGenres mediaId={params.id as string}/>
              </Suspense>
            </Section>
          </div>
          <div className="media-staf">
            <Section title="Staff">
              <Suspense fallback={<Loading />}>
                <LazyStaff mediaId={params.id as string}/>
              </Suspense>
            </Section>
          </div>
          <div className="media-review">
            <Section title="Reviews">
              <Suspense fallback={<Loading />}>
                <LazyReviews mediaId={params.id as string}/>
              </Suspense>
            </Section>
          </div>
          <div className="media-pro">
            {
              (userRoles?.includes(UserRoles.Pro) || userRoles?.includes(UserRoles.Administrator) || userRoles?.includes(UserRoles.SystemAdministrator)) &&              
              <Suspense fallback={<Loading />}>
                <LazyStatistics mediaId={params.id as string}/>
              </Suspense>
            }
          </div>
          <>
            {
              isRatingPopupOpen && 
              <RateMediaPopup mediaId={params.id as string}
                              rating={userMediaInfo?.rating}
                              onClose={ratingPopupHandler}
                              onRatingChange={ratingHandler}
              />
            }
            {
              isContentPopupOpen && 
              <ContentViewer mediaId={params.id as string} 
                             onClose={contentHandler}
              />
            }
          </>
        </div>   
      } 
    </>
  );
}