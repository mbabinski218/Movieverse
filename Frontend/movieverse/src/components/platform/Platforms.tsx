import { usePlatform } from "../../hooks/usePlatform";
import { Loading } from "../basic/Loading";
import { Text } from "../basic/Text";
import "./Platforms.css";

export interface PlatformProps {
  mediaId: string;
  className?: string;
}

export const Platforms: React.FC<PlatformProps> = ({mediaId, className}) => {
  const [platforms] = usePlatform(mediaId);

  return (
    <div className={`platform-menu ${className ? className : ""}`}>
      <div className="platform-title">Where to watch</div>
      {
        platforms ?
        platforms.map((platform, index) => (
          <div key={index}>
            <Text label={platform.name} 
                  text={`${platform.price.toString()}$`} 
            />
            {
              index !== platforms.length - 1 &&
              <hr className="platform-break" />
            }
          </div>
        )):
        <div className="platform-loading">
          <Loading />
        </div>
      }
    </div>
  )
}