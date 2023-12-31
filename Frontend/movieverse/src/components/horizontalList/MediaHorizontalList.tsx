import { useCallback, useEffect, useRef, useState } from "react";
import { FilteredMediaDto } from "../../core/dtos/media/filteredMediaDto";
import { MediaDemo } from "./MediaDemo";
import { WatchlistStatusDto } from "../../core/dtos/user/watchlistStatusDto";
import { Api } from "../../Api";
import "./MediaHorizontalList.css";
import leftArrow from "../../assets/arrow-left.svg";
import rightArrow from "../../assets/arrow-right.svg";
import { LocalStorage } from "../../hooks/useLocalStorage";

interface MediaHorizontalListProps {
  filteredMedia: FilteredMediaDto;
}

type ScrollButtons = {
  left: boolean;
  right: boolean;
}

export const MediaHorizontalList: React.FC<MediaHorizontalListProps> = ({filteredMedia}) => {    
  // Refs
  const containerRef = useRef<HTMLDivElement | null>(null);
  const itemRef = useRef<HTMLDivElement>(null);  
  
  // Scroll
  const [scrollPosition, setScrollPosition] = useState<number>(0);
  const [scrollButtons, setScrollButtons] = useState<ScrollButtons>({ left: false, right: false });
  
  // Watchlist statuses
  const [watchlistStatuses, setWatchlistStatuses] = useState<WatchlistStatusDto[]>([]);
  const [watchListLoaded, setWatchListLoaded] = useState<boolean>(false);

  // On container ref change
  const onContainerRefChange = useCallback((node: HTMLDivElement | null) => {
    if (node !== null) {    
      containerRef.current = node;

      const containerWidth = containerRef.current.offsetWidth;
      const screenWidth = window.innerWidth;
      setScrollButtons({ left: false, right: containerWidth > screenWidth });
    }
  }, [containerRef, setScrollButtons]);

  // Calculate scroll step
  const calculateScrollStep = (): number => {
    if (itemRef.current && containerRef.current) {
      const itemWidth = itemRef.current.offsetWidth;
      const windowWidth = window.innerWidth;
      
      return Math.floor(windowWidth / itemWidth) * itemWidth;
    }

    return 0;
  };

  // Left button scroll click
  const leftButtonScrollClick = useCallback(() => {
    const scrollStep = calculateScrollStep();

    if (scrollPosition > 0) {
      if (scrollPosition - scrollStep <= 0) {
        setScrollPosition(0);
        
        const containerWidth = containerRef.current?.offsetWidth || 0;
        const screenWidth = window.innerWidth;

        setScrollButtons({ left: false, right: containerWidth >= screenWidth });
      }
      else {
        setScrollPosition(scrollPosition - scrollStep);
        setScrollButtons({ left: true, right: true });
      }
    }
  }, [scrollPosition]);

  // Right button scroll click
  const rightButtonScrollClick = useCallback(() => {
    const scrollStep = calculateScrollStep();
    const containerWidth = containerRef.current?.offsetWidth || 0;

    const newPosition = scrollPosition + scrollStep;
    const maxPosition = containerWidth - scrollStep;

    if (newPosition >= maxPosition) {
      setScrollPosition(maxPosition);
      setScrollButtons({ left: true, right: false });
    }
    else {
      setScrollPosition(newPosition);
      setScrollButtons({ left: true, right: true });
    }    
  }, [scrollPosition]);

  // Recalculate on window resize
  useEffect(() => {
    const handleResize = () => {
      const scrollStep = calculateScrollStep();
      const containerWidth = containerRef.current?.offsetWidth || 0;

      setScrollButtons({ left: scrollButtons.left, right: scrollPosition + scrollStep < containerWidth}); 
    };

    window.addEventListener('resize', handleResize);
    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, [scrollButtons, scrollPosition]);

  // Watchlist statuses
  useEffect(() => {
    if (!filteredMedia.media?.items || !LocalStorage.getAccessToken()) {
      return;
    }

    Api.getWatchlistStatuses(filteredMedia.media?.items?.map(x => x.id) ?? [])
      .then(statuses => {
        setWatchListLoaded(true);
        setWatchlistStatuses(statuses)
      })
      .catch(err => console.error(err));
  }, []);

  // Update scroll buttons
  return (
    <div>
      <span className="platfromName">{filteredMedia.platformName}</span>
      <div className="mediaHorizontalList">          
        <div className={scrollButtons.left ? "scrollButton left" : "scrollButton disable"} 
             onClick={leftButtonScrollClick}
        >
          <img className="scrollButtonIcon" 
               src={leftArrow} 
               alt="left arrow" 
          />
        </div>
        <div className="mediaHorizontalListContainer" 
             style={{transform: `translateX(-${scrollPosition}px)`}} 
             ref={onContainerRefChange}
        >
          {filteredMedia.media?.items?.map((media) => (
            <div key={media.id} 
                 ref={itemRef}
            >
              <MediaDemo mediaDemo={media} 
                         isOnWatchlist={watchlistStatuses.find(x => x.mediaId === media.id)?.isOnWatchlist ?? null} 
                         isWatchlistLoaded={watchListLoaded}
              />
            </div>
          ))}
        </div>
        <div className={scrollButtons.right ? "scrollButton right" : "scrollButton disable"} 
             onClick={rightButtonScrollClick}
        >
          <img className="scrollButtonIcon" 
               src={rightArrow} 
               alt="right arrow" 
          />
        </div>      
      </div>
    </div>
  );
}